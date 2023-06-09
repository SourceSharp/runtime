using SourceSharp.Core.Models;

namespace SourceSharp.Core.Interfaces;

internal interface IConVarManager : IListenerBase
{
    public void OnConVarChanged(string name, string oldValue, string newValue);

    public CConVar? FindConVar(string name);
}
