FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY EstoqueLiaTattoo.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish EstoqueLiaTattoo.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV PORT=10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "EstoqueLiaTattoo.dll"]
