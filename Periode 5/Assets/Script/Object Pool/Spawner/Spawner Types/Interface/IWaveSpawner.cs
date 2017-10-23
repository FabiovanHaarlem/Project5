using System.Collections.Generic;

namespace Plugins.ObjectPool.Spawners
{
    public interface IWaveSpawner
    {
        List<SpawnItem> M_Wave { get; set; }
        float M_Timer { set; get; }
        float M_MaxTimer { get; set; }
    }
}
