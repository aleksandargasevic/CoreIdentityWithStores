# Custom Implementation of some  Identity Classes
## Net Core 2.2

Custom Implementation of IdentityRole, IdentityUser, RoleStore and UserStore in 

1. Instal MsSQL and Make Database
2. Be sure to input correct MsSQL Connection String to the AppSettings pointing to the correct Database
3. Run Commands for Migration to make tables to the MsSQL database:
```
  Add-Migration InitialMigration
```
Then:
```
  Update-Database
```
