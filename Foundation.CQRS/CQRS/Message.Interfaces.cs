

namespace Foundation.CQRS
{
    public interface ICommand
    {
    }

    public interface IExternalMessage
    {
    }

    public interface IQuery
    {
    }

    public interface IQuery<TQueryResult> : IQuery
    {
    }

    public interface IEvent
    {
    }
}