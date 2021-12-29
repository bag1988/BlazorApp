using ServiceGrpc;

namespace BlazorApp.Grpc.Data
{
    public interface IMessageInterface
    {
        public Task<List<MessageModel>> GetAllMessage(Param param);

        public Task<MessageModel> GetMessage(Param param);

        public Task<Param> SaveMessage(MessageModel mes);

        public Task<Param> DeleteMessage(Param param);
    }
}
