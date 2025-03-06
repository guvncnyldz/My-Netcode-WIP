using System;
using System.Linq.Expressions;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int key = 0;
    private void Start()
    {
        PlayerListener.GetInstance().Register(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*Expression<Action> action = () => Jump(50, "as");
            ClientUtils.StartSendingJsonToServerWithKey(action,key);*/
        }
    }

    public void Jump(int power, string other)
    {
        ICommand command = new SendMessageEvent(other,this);
        Invoker.GetInstance().AddCommand(command);
    }
}