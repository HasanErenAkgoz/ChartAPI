using ChartAPI.Hubs;
using ChartAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TableDependency.SqlClient;

namespace ChartAPI.Subscription
{

    public interface IDatabaseSubscription
    {
        void Configure(string tableName);
    }
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        SqlTableDependency<T> _tableDependency;
        IHubContext<SalesHub> _hubContext;
        IConfiguration _configuration;

        public DatabaseSubscription(IConfiguration configuration, IHubContext<SalesHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }
        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SQL"), tableName);
            _tableDependency.OnChanged += async (o, e) =>
            {

                ChartProjectContext chartProjectContext = new ChartProjectContext();
                var query = await (from personel in chartProjectContext.Staff
                                   join sales in chartProjectContext.Sales
                                   on personel.Id equals sales.StaffId
                                   select new
                                   {
                                       personel,
                                       sales
                                   }).ToListAsync();

                List<object> datas = new List<object>();
                var personelNameList = query.Select(d => d.personel.Name).Distinct().ToList();
                personelNameList.ForEach(o =>
                {
                    datas.Add(new
                    {
                        name = o,
                        data = query.Where(s => s.personel.Name == o).Select(s => s.sales.Price).ToList()

                    });
                });
                await _hubContext.Clients.All.SendAsync("receiveMessage", datas);

            };

            _tableDependency.OnError += (o, e) =>
            {

            };
            _tableDependency.Start();
        }

        private DatabaseSubscription() => _tableDependency.Stop();
    }
}
