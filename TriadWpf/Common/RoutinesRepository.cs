using System;
using System.Collections.Generic;
using TriadCore;
using TriadCore.Рутины.Базовые;
using TriadWpf.Common.Enums;
using TriadWpf.Models;

namespace TriadWpf.Common
{
    public class RoutineViewItem
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
                new RoutineViewItem("Получатель сообщений", RoutineType.Receiver),

                // Обработчик не реализован
                new RoutineViewItem("Обработчик заявок/сообщений", RoutineType.Undefined),

                new RoutineViewItem("Сеть петри, место", RoutineType.PetriNetPlace),
                new RoutineViewItem("Сеть петри, переход", RoutineType.PetriNetTransition),
                new RoutineViewItem("Сеть Петри, случайный переход", RoutineType.PetriNetPropabilityTransition),
                
                // Не реализовано
                new RoutineViewItem("Сеть петри, временной переход", RoutineType.Undefined),
                new RoutineViewItem("Сервер", RoutineType.Undefined),
                new RoutineViewItem("Клиент", RoutineType.Undefined),
                new RoutineViewItem("Роутер", RoutineType.Undefined)
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
                case RoutineType.PetriNetPlace:
                    return new PetriNetPlace(0, 1);
                case RoutineType.PetriNetTransition:
                    return new PetriNetTransition(1);
                case RoutineType.PetriNetPropabilityTransition:
                    return new PetriNetPropabilityTransition(1, 0.5);
                default:
                    throw new Exception("Такой тип рутины не определен");
            }
        }
    }
}
