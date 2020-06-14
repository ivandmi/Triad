using System;
using TriadCore;
using TriadWpf.Common.Interfaces;

namespace TriadWpf.Models
{
    public struct ParamMetadata
    {
        public string Description { get; }

        public string DisplayName { get; }

        public string Name { get; }

        public SpyObjectType Type { get; }

        public ParamMetadata(string description, string displayName, string name, SpyObjectType type)
        {
            Description = description;
            DisplayName = displayName;
            Name = name;
            Type = type;
        }
    }
}
