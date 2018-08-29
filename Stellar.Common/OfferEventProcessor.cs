using Common.Logging;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stellar.Common
{
    public class OfferEventProcessor : IEventProcessor
    {
        ILog logger = LogManager.GetLogger(typeof(OfferEventProcessor));
        Action<Offer> offerHandler;

        public OfferEventProcessor(Action<Offer> offerHandler)
        {
            this.offerHandler = offerHandler;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            logger.Info($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            logger.Info($"OfferEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            logger.Error($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                logger.Info($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");

                try
                {
                    var offer = JsonConvert.DeserializeObject<Offer>(data);

                    if (offerHandler != null && offer != null)
                        offerHandler(offer);
                }
                catch(Exception ex)
                {
                    logger.Error("Failed to deserialize offer.", ex);
                }
            }

            return context.CheckpointAsync();
        }
    }
}
