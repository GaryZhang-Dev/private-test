FROM /usr/dotnet/dotnet-3.1/dotnet
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app
EXPOSE 80

WORKDIR /app
COPY /publish ./
ENTRYPOINT ["dotnet", "AccountService.Api.dll"]