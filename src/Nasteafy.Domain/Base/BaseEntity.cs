using Nasteafy.Domain.Base;

namespace Nasteafy.Domain.Contracts
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
