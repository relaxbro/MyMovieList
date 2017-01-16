# MyMovieList

Keep track of movies you have watched and movies that you plan to watch.

## Current 

![Screenshot](/Screenshots/screenshot20170116.png)


### Ideas
 - Make list bigger, most important part of app
 - Hide unimplemented features
 - Implement TV series
 - Search disk or folder for movies
 - Implement reactive extensions

### Todo
 - Logging
 - Add sorting/showing by country. Add country and flag to view
 - Extend GetResponseAsync with timeout
 - Make taskbar icon show progress when redownloading all data
 - Automatically redownload data if no image can be fetched. Add setting maybe
 - Add showing released / not released
 - Automatically highlight searh in list when typing


### Version 2017.01.16
 - Automatically do search in search box, done with binding delay

### Version 2016.11.25
 - Make field for actors two lines
 - Remember seen / not seen option
 - Remember movie shown when closing program

### Version 2016.07.12
 - Implemented async/await web request
 - Implemented redownload of data for all movies
 - Search with imdb link
 - Added optional search argument year
 - Open previous file on startup setting
 - Fixed inconsistencies in my rating
 - Fixed redownloading data not updating view
 - Fixed bug with genre when opening a file
 - Fixed some web request timeout exceptions

### Version 2015.10.15
 - Added settings file. Can open previous list when starting application. Has to be edited manully for now.
 - Added random movie button.
 - Added dialog box when closing without saving.
 - Cleaned up some of the code. Still more to do.
 - Stopped serializing unwanted data to json.
 - Fixed starting new list, view is not empty, make sure old is saved first.
 - Search window focus set to textbox.
 - Fixed when json from OMDB contains errors. ex Fists of legend. Have to discard for now.
 - Various other bug fixes.

### Version 2015.09.21
 - Added user rating
 - Added sorting by title, imdb rating, user rating and year
 - Added viewing by genre
 - UI updated
 - Fixed UI scaling
 - Fixed keyboard shortcuts not working
 - Rotten Tomatoes included



