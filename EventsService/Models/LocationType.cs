namespace EventsService.Models;

public enum LocationType
{
    /// <summary>
    /// Можно создать объект локации, чтобы все пользователи могли ссылаться на него.
    /// Например, Парк Горького
    /// </summary>
    Location,
    /// <summary>
    /// Если пользователь не желает создавать/пользоваться объектом локации, он может просто указать координаты
    /// </summary>
    Coordinates
}
