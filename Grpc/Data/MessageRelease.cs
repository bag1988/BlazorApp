using Microsoft.EntityFrameworkCore;
using ServiceGrpc;

namespace BlazorApp.Grpc.Data
{
    public class MessageRelease:IMessageInterface, IDisposable
    {
        private readonly MessagesDbContext _context;

        public MessageRelease(MessagesDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageModel>> GetAllMessage(Param param)
        {

            return await _context.Messages.OrderBy(x => (param.SortBy == true ? EF.Property<object>(x, param.Sort) : 0))
                .ThenByDescending(x => (param.SortBy == false ? EF.Property<object>(x, param.Sort) : 0)).ToListAsync();
        }

        public async Task<MessageModel> GetMessage(Param id)
        {
            MessageModel mes = new();
            if (await _context.Messages.AnyAsync(x => x.Id == id.Id))
                mes = await _context.Messages.FirstOrDefaultAsync(x=> x.Id == id.Id);
            return mes;
        }

        public async Task<Param> SaveMessage(MessageModel mes)
        {            
            if(mes.Id == 0)
            {               
               await _context.Messages.AddAsync(mes);
            }
            else
            {
                _context.Messages.Update(mes);
            }

            await _context.SaveChangesAsync();

            Param id = new () { Id = mes.Id };
            return id;
        }

        public async Task<Param> DeleteMessage(Param id)
        {            
            if (await _context.Messages.AnyAsync(x => x.Id == id.Id))
            {                
                _context.Messages.Remove(await _context.Messages.FindAsync(id.Id));
            }
            await _context.SaveChangesAsync();
            return id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
    }
}
