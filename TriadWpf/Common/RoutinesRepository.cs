using System;
using System.Collections.Generic;
using TriadCore;
using TriadWpf.Common.Enums;

namespace TriadWpf.Common
{
    public struct RoutineViewItem
    {
        public string Name { get; private set; }
        public RoutineType Type { get; private set; }

        public RoutineViewItem(string name, RoutineType type)
        {
            Name = name;
            Type = type;
        }
    }
    class RoutinesRepository
    {
        public List<RoutineViewItem> RoutineMetadata { get; private set; }
        public RoutinesRepository()
        {
            // Наверное эту информацию нужно вынести в файл
            RoutineMetadata = new List<RoutineViewItem>
            {
                new RoutineViewItem("Генератор сообщений", RoutineType.MessegeGenerator),
                new RoutineViewItem("Получатель сообщений", RoutineType.Receiver)
            };
        }

        public Routine GetRoutine(RoutineType type)
        {
            switch (type)
            {
                case RoutineType.MessegeGenerator:
                    return new MessageGenerator();
                case RoutineType.Receiver:
                    return new Receiver();
                default:
                    throw new Exception("Такой тип рутины не определен");
            }
        }
    }
}
