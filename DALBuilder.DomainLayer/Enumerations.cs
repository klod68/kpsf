namespace DALBuilder.DomainLayer
{
     public enum ConcurrencySupportEnum
     {
        None,
        Optimistic,
        PessimisticUserId,
        PessimisticUserName
     }
}