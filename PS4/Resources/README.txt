Made by Ben Huenemann (9/18/2019)

The spreadsheet internals will be setup so that an empty
spreadsheet created that stores cells. The cells will be
stored in a subclass for convenience. The values in a
dictionary will be associated with those cells and the
keys will be associated with the cell names. Each
spreadsheet will also be associated with a dependency
graph that will keep track of each cell's dependencies.

This spreadsheet project builds on the versions of PS2
and PS3 that have been edited to pass the grading tests.