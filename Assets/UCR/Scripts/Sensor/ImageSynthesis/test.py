foreach(KeyValuePair<int, int> res in dict){
    if (res.Value == 5){
        dict.Remove(res.Key);
    }
}