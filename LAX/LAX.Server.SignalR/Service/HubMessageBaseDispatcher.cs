﻿using System.Diagnostics.CodeAnalysis;
using AiController.Abstraction.Operation;
using AiController.Operation.Operators;
using AiController.Server.SignalR.Interface;
using Microsoft.AspNetCore.SignalR;

namespace AiController.Server.SignalR.Service;

public abstract class HubMessageBaseDispatcher<THub, TOperator, TMessage>
    : IHubDispatchService<THub, TOperator, TMessage>
    where THub : Hub
    where TOperator : IAsyncOperator<TMessage>, IProxied<IAsyncOperator<TMessage>> 
{
    protected HubMessageBaseDispatcher(IExtensibleAsyncOperator<TMessage> asyncOperator)
    {
        AsyncOperator = asyncOperator;
    }
    private IExtensibleAsyncOperator<TMessage> AsyncOperator { get; }
    private Task<TMessage>? CurrentRequest { get; set; }

    private readonly Dictionary<string, Tuple<TOperator, THub>> connectedHubs = new();

    protected bool TryGetHubByConnectionId(string connectionId, [MaybeNullWhen(false)] out Tuple<TOperator, THub> value) =>
        connectedHubs.TryGetValue(connectionId, out value);

    protected IEnumerable<KeyValuePair<string,Tuple<TOperator,THub>>> GetHubByIdentifier(string identifier) => 
        connectedHubs
            .Where(pair => 
                pair.Value.Item1.Identifier.Trim() == identifier);

    protected Task<TMessage> Run(Func<TMessage> request)
    {
        CurrentRequest = CurrentRequest == null 
            ? Task.Run(request) 
            : CurrentRequest.ContinueWith(t => request());
        return CurrentRequest;
    }

    public void OnHubDisconnect(string connectionId)
    {
        if (!connectedHubs.TryGetValue(connectionId, out var field)) return;
        AsyncOperator.Remove(field.Item1);
        connectedHubs.Remove(connectionId);
    }
    public bool OnRegister(THub hub, TOperator identifier)
    {
        identifier.Proxy = AsyncOperator;
        if (connectedHubs.TryGetValue(hub.Context.ConnectionId, out var field))
        {
            AsyncOperator.Remove(field.Item1);
        }
        connectedHubs[hub.Context.ConnectionId] = new(identifier, hub);
        AsyncOperator.Add(identifier);
        return true;
    }
    public abstract Task OnReceiveMessage(THub hub, string message);
}