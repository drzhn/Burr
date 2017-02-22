using UnityEngine;
using System.Collections;

public class State
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;

    public State(string _name, Vector3 _position, Quaternion _rotation)
    {
        name = _name;
        position = _position;
        rotation = _rotation;
    }
    public State(GameObject obj)
    {
        name = obj.name;
        position = obj.transform.position;
        rotation = obj.transform.rotation;
    }

    public void SetState()
    {
        GameObject obj = GameObject.Find(name);
        // за одно изменение state мы изменяем либо пoзицию, либо вращение,
        // поэтому отправляем на обработку объекту одно из них.
        // оба отправлять не получалось, сраный баг, лень искать
        if (obj.transform.position != position)
            obj.SendMessage("SetTargetPosition", position);
        if (obj.transform.rotation != rotation)
            obj.SendMessage("SetTargetRotation", rotation);
    }

    public static bool operator ==(State a, State b)
    {
        if (a.name == b.name &&
            a.position == b.position &&
            a.rotation == b.rotation)
            return true;
        else
            return false;
    }
    public static bool operator !=(State a, State b)
    {
        if (a.name == b.name &&
            a.position == b.position &&
            a.rotation == b.rotation)
            return false;
        else
            return true;
    }

    public string ToString()
    {
        return name + position.ToString();
    }
}