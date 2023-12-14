const fs = require('fs');
String.prototype.splice = function(start, delCount, newSubStr) {
    return this.slice(0, start) + newSubStr + this.slice(start + Math.abs(delCount));
};


let galaxyLines = fs.readFileSync('./day11input1.txt').toString('utf-8').split('\r\n');

let galaxies = [];
let shortestPaths = [];

let emptyLines = {};
emptyLines.horizontal = [];
emptyLines.vertical = [];

FindEmpty();
//console.log(galaxies)
Expand(1);
//console.log(galaxies)

//FindGalaxies();
FindPaths();


let sum = 0;
shortestPaths.forEach(path => sum+= path);
console.log(sum);

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

function Expand(amount = 1)
{
    for (let i = 0; i < emptyLines.horizontal.length; i++)
    {
        let value = emptyLines.horizontal[i];
        for (let a = 0; a < amount; a++)
        {
            galaxyLines.splice(value, 0, '.'.repeat(galaxyLines[value].length));
            emptyLines.horizontal.splice(i, 0, value)
            galaxies.forEach(galaxy => {
                if (galaxy.y > value) galaxy.y++;
            })
            for (let followingGalaxies = i + 1; followingGalaxies < emptyLines.horizontal.length; followingGalaxies++)
            {
                emptyLines.horizontal[followingGalaxies] ++;

            }
        }
        i+=amount;
    }

    console.log("Expanded vertically")

    for (let i = 0; i < emptyLines.vertical.length; i++)
    {
        let value = emptyLines.vertical[i];
        for (let a = 0; a < amount; a++)
        {
            for (let j = 0; j < galaxyLines.length; j++)
            {
                galaxyLines[j] = galaxyLines[j].splice(value, 0, '.');
            }

            emptyLines.vertical.splice(i, 0, value);
            galaxies.forEach(galaxy => {
                if (galaxy.x > value) galaxy.x++;
            })
            for (let followingGalaxies = i + 1; followingGalaxies < emptyLines.vertical.length; followingGalaxies++)
            {
                emptyLines.vertical[followingGalaxies]++;
            }
        }
        i+=amount;
    }
}


function FindPaths()
{
    for (let i = 0; i < galaxies.length; i++)
    {
        for (let j = i + 1; j < galaxies.length; j++)
        {
            //console.log(`Finding shortest path betwee galaxy ${i + 1} and galaxy ${j + 1}... ${FindPath(galaxies[i], galaxies[j])}`)
            shortestPaths.push(FindPath(galaxies[i], galaxies[j]));
        }
    }
}

function FindPath(galaxy1, galaxy2)
{
    return Math.abs(galaxy2.x - galaxy1.x) + Math.abs(galaxy2.y - galaxy1.y)
}