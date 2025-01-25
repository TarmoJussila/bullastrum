using System;
using Bullastrum.Utility;

namespace Bullastrum.Gameplay
{
    public class GameController : Singleton<GameController>
    {
        public static Action<int> OnPopulationChanged;
        public static Action<int> OnProductionChanged;
        
        private int _population;
        private int _production;

        public void AddPopulation()
        {
            _population++;
            OnPopulationChanged?.Invoke(_population);
        }

        public void AddProduction()
        {
            _production++;
            OnProductionChanged?.Invoke(_production);
        }
    }
}