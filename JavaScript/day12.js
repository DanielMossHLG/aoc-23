const fs = require('fs');
String.prototype.splice = function(start, delCount, newSubStr) {
    return this.slice(0, start) + newSubStr + this.slice(start + Math.abs(delCount));
};

const permutations = {'1': ['.', '#']};

let possibilityTracker = []

Main();
Main(2);

console.log(possibilityTracker);

let part1 = 0;
let part2 = 0;

possibilityTracker.forEach(poss => {
    part1 += poss.oneLoop;

    part2 += (poss.oneLoop * Math.pow(poss.twoLoop / poss.oneLoop, 4))
});

console.log(`Part 1: ${part1} | Part 2: ${part2}`);

function Main(amountOfLoops = 1)
{
    let lines = fs.readFileSync('./day12input1.txt').toString('utf-8').split('\r\n');
    for (let i = 0; i < 1; i++)
    {
        let split = lines[i].split(' ');
        lines[i] = {data: split[0], segments: split[1].split(',').map(x => +x), splitData: []};

        
        if (amountOfLoops > 1) lines[i].data = (lines[i].data + '?').repeat(amountOfLoops).slice(0, -1);
        for (let j = 1; j < amountOfLoops; j++)
        {
            lines[i].segments = lines[i].segments.concat(lines[i].segments);
        }
        let currString = "";
        for (let j = 0; j < lines[i].data.length; j++)
        {
            if (currString.length == 0)
            {
                currString = lines[i].data[j];
                continue;
            }
            
            if (lines[i].data[j] != currString[0])
            {
                lines[i].splitData.push(currString);
                currString = "";
            }
            
            currString+= lines[i].data[j];
            
            if (j == lines[i].data.length - 1) lines[i].splitData.push(currString);
        }
        
        for (let j = 0; j < lines[i].splitData.length; j++)
        {
            if (lines[i].splitData[j][0] == "?") lines[i].splitData[j] = GetPermutations(lines[i].splitData[j].length);
            else lines[i].splitData[j] = [lines[i].splitData[j]];
        }

        console.log(lines[i])
        
        let sum = 0;
        FindPossibleLines(lines[i]).forEach(l => {
            if (CheckValid(l, lines[i].segments))
            sum++;
        });

        if (possibilityTracker[i])
            possibilityTracker[i].twoLoop = sum;
        else
        possibilityTracker[i] = {oneLoop: sum};
    }
    
    
}

function FindPossibleLines(line)
{
    let possibleLines = line.splitData[0];

    for (let i = 1; i < line.splitData.length; i++)
    {
        let temp = [];
        possibleLines.forEach(currLines => 
            {
                line.splitData[i].forEach(nextLine => {
                    temp.push(currLines + nextLine);
                })
            })

        possibleLines = temp;
    }

    console.log(possibleLines.length);

    return possibleLines;
}

function GetPermutations(length)
{
    if (permutations[length]) return permutations[length];

    let oneLower = GetPermutations(length - 1);
    let newArr = [];

    oneLower.forEach(permutation =>
    {
        newArr.push(permutation + '.');
        newArr.push(permutation + '#');
    })

    permutations[length] = newArr;
    return permutations[length];
}


function CheckValid(line, segments)
{
    let check = line.split('.').filter(x => x != '');
    if (check.length != segments.length) return false;

    for (let i = 0; i < check.length; i++)
    {
        if (check[i].length != segments[i]) return false;
    }
    return true;
}