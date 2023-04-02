using LAX.Abstraction.Communication;
using LAX.Communication.GPT35;
using LAX.Operation.Operators;
using LAX.Operation.Operators.Direct;
using LAX.Operation.Operators.Indirect;
using LAX.Transmission.Json;
using OpenAI.Chat;

namespace LAX.Test
{
    public class Tests
    {
        private IAsyncCommunicator<ChatPrompt[]> Communicator { get; set; }
        private IExtensibleAsyncOperator<DistributeMessageModel?> Server { get; set; }
        private Gpt35ClientOperator<DistributeMessageModel?> Client { get; set; }

        [SetUp]
        public void Setup()
        {
            Communicator = new Gpt35AsyncCommunicator()
            {
               Temperature = 0,
               ModelName = ""
            };
            Server = new Gpt35DistributeAsyncOperator<DistributeMessageModel>(Communicator)
            {
                Identifier = "You are center server :[Server]",
            };
            Server.Add(Client = new Gpt35ClientOperator<DistributeMessageModel?>()
            {
                Identifier = "Client1",
                Description = "This is my client"
            });
            Client.Proxy = Server;

            string str = " ";
            var bytes = " "u8.ToArray();
        }

        [Test]
        public async Task Test1()
        {
            var clientMessage = "������Ҫ��Ļظ�����ת���ɼ�̵������� \"����һ̨�豸�е� d:MyDir Ŀ¼�´���һ����ΪAccess��Ŀ¼\" ";
            try
            {
                var res = await Client.SendAsync(clientMessage);
                Assert.NotNull(res);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            Assert.Pass();
        }
    }
}