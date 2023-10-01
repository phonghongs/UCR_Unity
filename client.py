# Import socket module
import socket
import cv2
import numpy as np
import json

MODE_1 = 185
MODE_2 = 203
MODE_3 = 31

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


if __name__ == "__main__":
    try:
        while True:
            # CMD 1
            s.sendall(jsonObject(MODE_1))
            data = s.recv(255)
            y = json.loads(data)
            print(y)

            # CMD 2
            # s.sendall(jsonObject(MODE_2))
            # data = s.recv(100000)
            # try:
            #     image = cv2.imdecode(
            #         np.frombuffer(
            #             data,
            #             np.uint8
            #         ), -1
            #     )
            #     print(image.shape)
            #     cv2.imshow("RAW", image)
            # except Exception as er:
            #     print(er)
            #     pass

            # CMD 2
            s.sendall(jsonObject(MODE_3))
            data = s.recv(100000)
            try:
                image = cv2.imdecode(
                    np.frombuffer(
                        data,
                        np.uint8
                    ), -1
                )
                print(image.shape)
                cv2.imshow("SEG", image)
            except Exception as er:
                print(er)
                pass

            # maxspeed = 90, max steering angle = 25
            AVControl(speed=-10, angle=-10)

            key = cv2.waitKey(1)
            if key == ord('q'):
                break

    finally:
        print('closing socket')
        s.close()
