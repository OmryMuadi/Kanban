using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        [JsonInclude]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object ReturnValue { get; set; }

        [JsonInclude]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool ErrorOccurd { get => ErrorMessage != null; }
        



        public Response() { }

        public Response(string message)
        {
            ErrorMessage = message;
        }


        public Response(object obj)
        {
            ReturnValue = obj;
        }


        public Response(string message, object value)
        {
            ErrorMessage = message;
            this.ReturnValue = value;

        }
    }
}

