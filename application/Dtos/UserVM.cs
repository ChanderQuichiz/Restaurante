namespace application.Dtos;

public record UserVM(
    List<UserDto> users,
    int page,
    int totalPages,
    FilterUserDto filtros
);
