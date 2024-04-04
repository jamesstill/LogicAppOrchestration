using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SourceApi.Models;
using Azure.Messaging.ServiceBus;

namespace SourceApi.Repository
{
    public class WidgetContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public WidgetContext(DbContextOptions<WidgetContext> options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
            this.ChangeTracker.StateChanged += new EventHandler<EntityStateChangedEventArgs>(OnStateChanged);
            this.ChangeTracker.Tracked += new EventHandler<EntityTrackedEventArgs>(OnTracked);
        }

        public DbSet<Widget> Widgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WidgetMap());
            base.OnModelCreating(modelBuilder);
        }

        private void OnTracked(object? sender, EntityTrackedEventArgs e)
        {
            // Method intentionally left empty.
        }

        private void OnStateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            PayloadDto payload = CreatePayload(e);
            string body = JsonSerializer.Serialize(payload);
            string entityName = GetEntityName(e.Entry);

            ServiceBusMessage message = new(body)
            {
                ContentType = "application/json",
                Subject = entityName,
                SessionId = "ilp"
            };

            SendMessageAsync(message).Wait();
        }

        private async Task SendMessageAsync(ServiceBusMessage message)
        {
            string topicName = "ilp";
            string cn = _configuration.GetConnectionString("ServiceBusConnectionString") ?? string.Empty;
            
            ServiceBusClient client = new(cn);
            ServiceBusSender sender = client.CreateSender(topicName);

            try
            {
                await sender.SendMessageAsync(message);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        private static PayloadDto CreatePayload(EntityStateChangedEventArgs e)
        {
            return new PayloadDto()
            {
                PayloadAction = GetEntityState(e.OldState),
                PayloadType = GetEntityName(e.Entry),
                PayloadJSON = JsonSerializer.Serialize(e.Entry.Entity)
            };
        }

        private static string GetEntityName(EntityEntry e) => e.Entity.GetType().Name;

        private static string GetEntityState(EntityState state)
        {
            string entityState = string.Empty;
            switch (state)
            {
                case EntityState.Deleted:
                    entityState = "Deleted";
                    break;

                case EntityState.Modified:
                    entityState = "Modified";
                    break;

                case EntityState.Added:
                    entityState = "Added";
                    break;
            }

            return entityState;
        }
    }
}
