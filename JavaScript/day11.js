const fs = require('fs');

let galaxyLines = fs.readFileSync('./day11input1.txt').toString('utf-8').split('\r\n');

let galaxies = [];

let emptyLines = {};
emptyLines.horizontal = [];
emptyLines.vertical = [];

FindEmpty();
FindPaths();

let sumBase = 0;
let sumExp = 0;
galaxies.forEach(galaxy => {
    galaxy.shortestPaths.forEach(path => {
        sumBase += path.baseLength ;
        sumExp += path.expandingLines;
    })
})
console.log(`Part 1: ${sumBase + sumExp} | ${sumBase + (sumExp * 999999)}`);


function FindEmpty()
{
    let i = 0;
    for (let j = 0; j <galaxyLines[0].length; j++)
    {
        let isEmpty = true;
        for (i = 0; i < galaxyLines.length; i++)
        {
            if (galaxyLines[i][j] == '#') {
                galaxies.push({x: j, y: i});
                isEmpty = false;
            }
            if (j != 0) continue;
            if (galaxyLines[i].indexOf("#") == -1)
            {
                emptyLines.horizontal.push(i);
            }
        }
        if (isEmpty)
        {
            emptyLines.vertical.push(j)
        }
    }
}

function FindPaths()
{
    for (let i = 0; i < galaxies.length; i++)
    {
        galaxies[i].shortestPaths = [];
        for (let j = i + 1; j < galaxies.length; j++)
        {
            let pathdata = {baseLength:FindPath(galaxies[i], galaxies[j]), expandingLines: 0};
            pathdata.from = `(${galaxies[i].x}, ${galaxies[i].y})`;
            pathdata.to = `(${galaxies[j].x}, ${galaxies[j].y})`;

            emptyLines.horizontal.forEach(index => {
                if ((index > galaxies[i].y && index < galaxies[j].y) || (index < galaxies[i].y && index > galaxies[j].y))
                pathdata.expandingLines++;
            })
            emptyLines.vertical.forEach(index => {
                if ((index > galaxies[i].x && index < galaxies[j].x) || (index < galaxies[i].x && index > galaxies[j].x))
                pathdata.expandingLines++;
            })
            galaxies[i].shortestPaths.push(pathdata);
        }
    }
}

function FindPath(galaxy1, galaxy2)
{
    return Math.abs(galaxy2.x - galaxy1.x) + Math.abs(galaxy2.y - galaxy1.y)
}