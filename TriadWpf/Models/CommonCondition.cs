using System.Collections.Generic;
using TriadCore;

namespace TriadWpf.Models
{
    class CommonCondition : ICondition
    {
        public CommonCondition()
        {
            NameIdDictionary = new Dictionary<string, int>();
        }

        public int ProceduresCount
        {
            get => GetIProcedureCount();
        }

        public void AddProcedure(string name, IProcedure procedure)
        {
            NameIdDictionary.Add(name, ProceduresCount);
            AddIProcedure(procedure, ProceduresCount); 
        }

        public double TerrminateTime
        {
            get; set;
        }

        public override bool DoCheck(double SystemTime)
        {
            return TerrminateTime > SystemTime;
        }

        public Dictionary<string, IProcedure> GetProcedures()
        {
            // Я сделал так, потому что решил следовать принципу, что уже протестили, лучше не менять.
            // Возможно было бы лучше сделать и в ICondition словарь с ключом по имени, а не по id.
            // С учетом, что как раз нельзя на triad вроде как объявлять условия моделирования с одним именем.
            // Но мне грубо говоря через неделю диплом сдавать, поэтому так.
            // Это бы пришлось менять код в TriadCore, а потом в TriadCompiler.
            // На заметку если кто-то решит допилить

            Dictionary<string, IProcedure> dict = new Dictionary<string, IProcedure>();
            foreach(var pair in NameIdDictionary)
            {
                dict.Add(pair.Key, GetIProcedure(pair.Value));
            }

            return dict;
        }

        public override void DoInitialize()
        {
            InitializeAllIProcedure();
        }

        private Dictionary<string, int> NameIdDictionary;
    }
}
