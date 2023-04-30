using SourceSharp.Core.Bridges;

namespace SourceSharp.Core.Interfaces;

internal interface IConVarManager : IListenerBase
{
    public void OnConVarChanged(SSConVar conVar, string oldValue, string newValue);
}
