using SolutionTemplate.Interfaces.Base.Entities;

namespace SolutionTemplate.Domain
{
    public class TestDataValue : INamedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }
    }
}
