//THIS FILE IS AUTOGENERATED BY GHOSTCOMPILER. DON'T MODIFY OR ALTER.
using AOT;
using Unity.Burst;
using Unity.Networking.Transport;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;


namespace Assembly_CSharp.Generated
{
    [BurstCompile]
    public struct SendClientGameRpcSerializer : IComponentData, IRpcCommandSerializer<SendClientGameRpc>
    {
        public void Serialize(ref DataStreamWriter writer, in RpcSerializerState state, in SendClientGameRpc data)
        {
            writer.WriteInt((int) data.levelWidth);
            writer.WriteInt((int) data.levelHeight);
            writer.WriteInt((int) data.levelDepth);
            writer.WriteFloat(data.playerForce);
            writer.WriteFloat(data.bulletVelocity);
        }

        public void Deserialize(ref DataStreamReader reader, in RpcDeserializerState state,  ref SendClientGameRpc data)
        {
            data.levelWidth = (int) reader.ReadInt();
            data.levelHeight = (int) reader.ReadInt();
            data.levelDepth = (int) reader.ReadInt();
            data.playerForce = reader.ReadFloat();
            data.bulletVelocity = reader.ReadFloat();
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(RpcExecutor.ExecuteDelegate))]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<SendClientGameRpcSerializer, SendClientGameRpc>(ref parameters);
        }

        static PortableFunctionPointer<RpcExecutor.ExecuteDelegate> InvokeExecuteFunctionPointer =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);
        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return InvokeExecuteFunctionPointer;
        }
    }
    class SendClientGameRpcRpcCommandRequestSystem : RpcCommandRequestSystem<SendClientGameRpcSerializer, SendClientGameRpc>
    {
        [BurstCompile]
        protected struct SendRpc : IJobEntityBatch
        {
            public SendRpcData data;
            public void Execute(ArchetypeChunk chunk, int orderIndex)
            {
                data.Execute(chunk, orderIndex);
            }
        }
        protected override void OnUpdate()
        {
            var sendJob = new SendRpc{data = InitJobData()};
            ScheduleJobData(sendJob);
        }
    }
}