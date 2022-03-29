using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public Color Color;

    public interface IMember 
    {
        void ReceiveCommand(ICommand command);
    }

    public interface ICommand { }

    public IReadOnlyList<IMember> Members => _members;

    readonly List<IMember> _members = new List<IMember>();

    public void Join(IMember member) => _members.Add(member);

    public void Leave(IMember member) => _members.Remove(member);

    public void SendCommand(ICommand command) => _members.ForEach(it => it.ReceiveCommand(command));
   
}
