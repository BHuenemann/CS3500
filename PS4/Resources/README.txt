Made by Ben Huenemann

(9/27/2019)

The spreadsheet internals have been uupdated so that it
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