namespace Domain.Factory;

using Domain.Model;

public interface IColaboratorIdFactory
{
    ColaboratorId NewColaboratorId(long colaboratorId);
}