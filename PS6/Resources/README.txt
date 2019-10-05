Made by Ben Huenemann and Jonathan Wigderson

(10/4/2019)

Now a GUI has been added that has an internal spreadsheet
class item. This GUI updates the spreadsheet as the cell
contents are changed and updates the visuals when the
values in the spreadsheet are changed. It also has a file
system that can handle opening and saving files. The new
button opens a new window of the spreadsheet on the same
thread. If that window closes, it just modifies the window
count but doesn't close the form. This also provides warnings
for overwriting files and closing unsaved files. There is
also a help menu that describes the features in the
spreadsheet so the user can learn how to use it.

The class also keeps track of whether or not a file has been
changed and indicates that with an asterisk in the title.
Additionally, it also has a save button that saves to the
previous file and you can do ctrl+z to undo one change that
you made in the file.


Made by Ben Huenemann

(9/27/2019)

The spreadsheet internals have been updated so that it
also stores values of the cells now. Whenever a cell is
set to something, the value also updates. It also has
been updated to support XML saving and loading. The
biggest change in the way it's accessed is that setting
the contents of the cell only takes in a string now. If
the user wants to put in a formula, the string must start
with "=".

This spreadsheet project is now an updated version of
PS4. It was created in a separate branch and then merged.


(9/18/2019)

The spreadsheet internals will be setup so that an empty
spreadsheet created that stores cells. The cells will be
stored in a subclass for convenience. The values in a
dictionary will be associated with those cells and the
keys will be associated with the cell names. Each
spreadsheet will also be associated with a dependency
graph that will keep track of each cell's dependencies.

This spreadsheet project builds on the versions of PS2
and PS3 that have been edited to pass the grading tests.