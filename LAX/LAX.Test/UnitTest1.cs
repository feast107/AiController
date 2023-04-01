using AiController.Abstraction.Communication;
using AiController.Abstraction.Operation;
using AiController.Communication.GPT35;
using AiController.Operation.Operators.Direct;
using AiController.Transmission.SignalR;
using OpenAI.Chat;

namespace AiController.Test
{
    public class Tests
    {
        private IAsyncCommunicator<ChatPrompt[]> Communicator { get; set; }
        private IAsyncOperator<DistributeMessageModel?> Operator { get; set; }
        [SetUp]
        public void Setup()
        {
            Communicator = new Gpt35AsyncCommunicator()
            {
                
            };
            Operator = new Gpt35DistributeAsyncOperator<DistributeMessageModel>(Communicator)
            {
                Identifier = "�������ķ�����:[Server]",
                Description = """
                ������ [comp1,comp2] 2̨�豸���ڽ������Ŀͻ��˷���������У����ʹ��JSON��ʽ���лظ������磺
                { "device" :"comp1" , "reply":"mkdir d:" }��
                ����ʽ��ע������ָ�����豸�����뵽JSON��device��,���Ž��ظ��ͻ��˵����ݰ��տͻ��˵�Ҫ�����뵽reply�У�
                ������Σ��벻Ҫ�ظ�����JSON�ı����������
                """
            };
        }

        [Test]
        public async Task Test1()
        {
            var groupContext = """
                ������ [comp1,comp2] 2̨�豸���ڽ������Ļظ��У����ʹ��JSON��ʽ���лظ������� { "device" :"comp1" , "reply":"mkdir d:" }
                ����ʽ��ע������ָ�����豸�����뵽JSON��device��,���Ž��ظ����ֶΰ���Ҫ�����뵽reply�У�������Σ��벻Ҫ�ظ�����JSON�ı����������
                """;
            var groupMessage = "������������Ϣ���Կͻ��� comp1 ��\n";
            var clientMessage = "������Ҫ��Ļظ�����ת���ɼ�̵������У�\"����һ̨�豸�е� d:MyDir Ŀ¼�´���һ����ΪAccess��Ŀ¼\" ";
            try
            {
                var res = await Operator.SendAsync(
                    groupMessage +
                    clientMessage);
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