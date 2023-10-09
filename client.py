# Import socket module
import socket
import cv2
import numpy as np
import json

MODE_1 = 185
MODE_2 = 203
MODE_3 = 31
MAX_DGRAM = 2**16
# Create a socket object
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Define the port on which you want to connect
PORT = 11000
# connect to the server on local computer
s.connect(('127.0.0.1', PORT))

global speedcmd, anglecmd
speedcmd = 0
anglecmd = 0


def jsonObject(cmd=MODE_1):
    cmt = {}
    if cmd == MODE_1:
        cmt['Cmd'] = cmd
        cmt['Speed'] = speedcmd
        cmt['Angle'] = anglecmd
    else:
        cmt['Cmd'] = cmd
    return bytes(str(cmt), "utf-8")


def AVControl(speed, angle):
    global speedcmd, anglecmd
    speedcmd = speed
    anglecmd = angle


def GetData(mode):
    if mode == MODE_1 or mode == MODE_2 or mode == MODE_3:
        s.sendall(jsonObject(mode))
        bs = s.recv(8)
        length = int.from_bytes(bs, "little")

        data = b''
        while len(data) < length:
            to_read = length - len(data)
            data += s.recv(MAX_DGRAM if to_read > MAX_DGRAM else to_read)

        if (mode == MODE_1):
            y = json.loads(data)
            return y
        elif (mode == MODE_2 or mode == MODE_3):
            image = cv2.imdecode(
                np.frombuffer(
                    data,
                    np.uint8
                ), -1
            )
            return image


if __name__ == "__main__":
    try:
        while True:
            state = GetData(MODE_1)
            raw_image = GetData(MODE_2)
            segment_image = GetData(MODE_3)

            print(state)
            cv2.imshow('raw_image', raw_image)
            cv2.imshow('segment_image', segment_image)

            # maxspeed = 90, max steering angle = 25
            AVControl(speed=-10, angle=-10)

            key = cv2.waitKey(1)
            if key == ord('q'):
                break

    finally:
        print('closing socket')
        s.close()
