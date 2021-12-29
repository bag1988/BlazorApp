using Grpc.Core;
using ServiceGrpc;
using BlazorApp.Grpc.Data;

namespace BlazorApp.Grpc.Services
{
    public class MessageService : MessageContext.MessageContextBase
    {
        
        private readonly IMessageInterface _ImessageInterface;
        public MessageService(IMessageInterface ImessageInterface)
        {           
            _ImessageInterface = ImessageInterface;
        }

       
        public override async Task<MessageModel> GetMessag(Param param, ServerCallContext context)
        {
            return await _ImessageInterface.GetMessage(param);
        }

        public override async Task<Messages> GetMessages(Param param, ServerCallContext context)
        {
            Messages ListMessage = new();
            ListMessage.Items.AddRange(await _ImessageInterface.GetAllMessage(param));
            return ListMessage;
        }

        public override async Task<Param> SaveMesage(MessageModel mes, ServerCallContext context)
        {
            var param = await _ImessageInterface.SaveMessage(mes);

            if (string.IsNullOrEmpty(mes.UrlFile))
            {                
                mes.UrlFile = $"{param.Id}.webm";
                await _ImessageInterface.SaveMessage(mes);
            }

            return param;
        }

        public override async Task<Param> Delete(Param param, ServerCallContext context)
        {
            return await _ImessageInterface.DeleteMessage(param);
        }

        public override async Task<Empty> SaveFile(FileBase audioBase, ServerCallContext context)
        {
            try
            {               
                if (!Directory.Exists("wwwroot/wav"))
                    Directory.CreateDirectory("wwwroot/wav");

                if (File.Exists($"wwwroot/wav/{audioBase.Id}.webm"))
                    File.Delete($"wwwroot/wav/{audioBase.Id}.webm");

                await File.WriteAllBytesAsync($"wwwroot/wav/{audioBase.Id}.webm", Convert.FromBase64String(audioBase.File));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new Empty();
        }
    }
}
