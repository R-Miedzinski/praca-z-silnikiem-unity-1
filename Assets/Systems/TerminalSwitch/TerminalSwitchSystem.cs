using System;
using System.Collections.Generic;
using UnityEngine;

public class TerminalSwitchSystem : MonoBehaviour
{
  [Serializable]
  private class TerminalHandlerBinding
  {
    public TerminalData terminalData = null;
    public MonoBehaviour handlerComponent = null;
  }

  private static TerminalSwitchSystem activeSystem;

  [SerializeField] private List<TerminalHandlerBinding> handlerBindings = new List<TerminalHandlerBinding>();

  private readonly Dictionary<TerminalData, ITerminalHandler> handlersByTerminal = new Dictionary<TerminalData, ITerminalHandler>();
  private bool isSubscribed;

  private void OnEnable()
  {
    if (activeSystem != null && activeSystem != this)
    {
      Debug.LogWarning("Another TerminalSwitchSystem is already active. This instance will not subscribe to terminal events.", this);
      enabled = false;
      return;
    }

    activeSystem = this;
    RebuildHandlerMap();
    Subscribe();
  }

  private void OnDisable()
  {
    Unsubscribe();

    if (activeSystem == this)
    {
      activeSystem = null;
    }
  }

  private void Subscribe()
  {
    if (isSubscribed)
    {
      return;
    }

    // TerminalEdit.TerminalInteracted -= HandleTerminalInteracted;
    TerminalEdit.TerminalInteracted += HandleTerminalInteracted;
    isSubscribed = true;
  }

  private void Unsubscribe()
  {
    if (!isSubscribed)
    {
      return;
    }

    TerminalEdit.TerminalInteracted -= HandleTerminalInteracted;
    isSubscribed = false;
  }

  private void RebuildHandlerMap()
  {
    handlersByTerminal.Clear();

    if (handlerBindings == null || handlerBindings.Count == 0)
    {
      Debug.LogWarning("TerminalSwitchSystem has no terminal handler bindings.", this);
      return;
    }

    foreach (TerminalHandlerBinding binding in handlerBindings)
    {
      RegisterBinding(binding);
    }
  }

  private void RegisterBinding(TerminalHandlerBinding binding)
  {
    if (binding == null)
    {
      Debug.LogWarning("TerminalSwitchSystem found an empty terminal handler binding.", this);
      return;
    }

    if (binding.terminalData == null)
    {
      Debug.LogWarning("TerminalSwitchSystem binding is missing TerminalData.", this);
      return;
    }

    if (binding.handlerComponent == null)
    {
      Debug.LogWarning($"TerminalSwitchSystem has no handler assigned for terminal '{GetTerminalLabel(binding.terminalData)}'.", this);
      return;
    }

    if (binding.handlerComponent is not ITerminalHandler handler)
    {
      Debug.LogError($"TerminalSwitchSystem handler '{binding.handlerComponent.name}' for terminal '{GetTerminalLabel(binding.terminalData)}' does not implement ITerminalHandler.", binding.handlerComponent);
      return;
    }

    if (handlersByTerminal.ContainsKey(binding.terminalData))
    {
      Debug.LogWarning($"TerminalSwitchSystem has duplicate bindings for terminal '{GetTerminalLabel(binding.terminalData)}'. The first binding will be used.", this);
      return;
    }

    handlersByTerminal.Add(binding.terminalData, handler);
  }

  private void HandleTerminalInteracted(TerminalData terminalData, PlayerCharacter player)
  {
    if (terminalData == null)
    {
      Debug.LogWarning("TerminalSwitchSystem received a terminal interaction without TerminalData.", this);
      return;
    }

    if (player == null)
    {
      Debug.LogWarning($"TerminalSwitchSystem received terminal '{GetTerminalLabel(terminalData)}' without a PlayerCharacter reference.", this);
    }

    if (!handlersByTerminal.TryGetValue(terminalData, out ITerminalHandler handler) || handler == null)
    {
      Debug.LogWarning($"TerminalSwitchSystem has no handler registered for terminal '{GetTerminalLabel(terminalData)}'.", this);
      return;
    }

    handler.HandleTerminal(terminalData, player);
  }

  private string GetTerminalLabel(TerminalData terminalData)
  {
    if (terminalData == null)
    {
      return "Missing TerminalData";
    }

    if (!string.IsNullOrWhiteSpace(terminalData.TerminalName))
    {
      return terminalData.TerminalName;
    }

    if (!string.IsNullOrWhiteSpace(terminalData.TerminalId))
    {
      return terminalData.TerminalId;
    }

    return terminalData.name;
  }
}
