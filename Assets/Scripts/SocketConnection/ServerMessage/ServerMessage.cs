using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class ServerMessage 
{
    public static ServerMessage ParseData(JObject data)
    {
        ServerMessage serverMessage = new ServerMessage();

        serverMessage.method = ServerMessageUtils.ExtractAndRemoveData(data, "Method");
        serverMessage.key = ServerMessageUtils.ExtractAndRemoveData(data, "Id");
        serverMessage.@class = ServerMessageUtils.ExtractAndRemoveData(data, "Class");
        serverMessage.values = ServerMessageUtils.ExtractJValues(data);

        return serverMessage;
    }

    public string method;
    public string key;
    public string @class;
    public Dictionary<string, JValue> values;
}
