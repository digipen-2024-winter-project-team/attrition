using System;
using System.Collections.Generic;
using System.Text;

namespace Attrition.NameGeneration
{
    public class NameGenerator
    {
        private readonly List<string> prefixes;
        private readonly List<string> roots;
        private readonly List<string> suffixes;
        private readonly Random random;

        public NameGenerator(INameData nameData, int? seed = null)
        {
            if (nameData == null)
            {
                throw new ArgumentNullException(nameof(nameData), "NameData cannot be null.");
            }

            this.prefixes = nameData.Prefixes ?? throw new ArgumentNullException(nameof(nameData.Prefixes));
            this.roots = nameData.Roots ?? throw new ArgumentNullException(nameof(nameData.Roots));
            this.suffixes = nameData.Suffixes ?? throw new ArgumentNullException(nameof(nameData.Suffixes));
            this.random = seed.HasValue ? new(seed.Value) : new Random();
        }

        public string GenerateName()
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append(this.GetRandomElement(this.prefixes));
            stringBuilder.Append(this.GetRandomElement(this.roots));
            stringBuilder.Append(this.GetRandomElement(this.suffixes));

            return stringBuilder.ToString();
        }

        private string GetRandomElement(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("List is empty or null.");
            }

            return list[this.random.Next(list.Count)];
        }
    }
}
