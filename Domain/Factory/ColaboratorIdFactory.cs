namespace Domain.Factory;

using Domain.Model;

public class ColaboratorIdFactory: IColaboratorIdFactory
{
    public ColaboratorId NewColaboratorId(long colaboratorId)
    {
        return new ColaboratorId(colaboratorId);
    }
}