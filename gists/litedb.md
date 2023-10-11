```xaml
var c = database.GetLiteDatabase()
                            .GetCollection(Constants.Name_Compose);
            var all = c.FindAll();
```