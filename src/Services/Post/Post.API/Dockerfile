FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Services/Post/Post.API/Post.API.csproj", "src/Services/Post/Post.API/"]
COPY ["src/Services/Post/Post.Infrastructure/Post.Infrastructure.csproj", "src/Services/Post/Post.Infrastructure/"]
COPY ["src/BuildingBlocks/Services/ConsulService/ConsulService.csproj", "src/BuildingBlocks/Services/ConsulService/"]
RUN dotnet restore "src/Services/Post/Post.API/Post.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Post/Post.API"
RUN dotnet build "Post.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Post.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Post.API.dll"]