# Million.Technical.Test

To run the application, you need to apply the migrations to the database. The application is configured to point to a local SQL Server instance.

**Connection string:**
```
"DefaultConnection": "Server=localhost;Database=RealEstateDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

To apply the migrations, run the following commands in the solution's terminal:

1. Install `dotnet-ef` tool if not already installed:

```
dotnet tool install --global dotnet-ef --version 8.0.8
```

2. Update the database:

```
dotnet ef database update --project .\Million.Technical.Test.Infrastructure\Million.Technical.Test.Infrastructure.csproj --startup-project .\Million.Technical.Test\Million.Technical.Test.Api.csproj
```

### Next Steps:

1. **Create an Owner**:  
   You will need the Owner’s ID to create a property.

2. **Create a Property**:  
   Once the Owner is created, you can proceed with creating the property.

After creating the property, you can use the following commands and queries for additional functionalities:

### Endpoints

#### Owner

- **POST**  
  `/api/owner/create_owner` - Used to create owners.

#### Property

- **POST**  
  `/api/create_property` - Used to create properties.

- **POST**  
  `/api/add_property_image` - Used to add images to properties.

- **POST**  
  `/api/create_property_trace` - Used to add property traces to properties.

- **PUT**  
  `/api/change_property_price` - Used to change the price of a property.

- **PUT**  
  `/api/update_property` - Used to modify the values of a property. You can also create property traces from this endpoint.

- **GET**  
  `/api/property/{propertyId}/image/{imageId}` - Used to retrieve images for a property.

- **GET**  
  `/api/get_properties_by_filters` - Used to retrieve properties based on filters.

### Notes

- The code is mostly self-documenting, so there are few comments in the code itself.
- However, controllers include comments regarding their functionalities.
