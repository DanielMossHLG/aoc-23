const fs = require('fs');

var input = fs.readFileSync('./day1input1.txt').toString('utf-8');

input = input.split('\n');

//Part 1
var output1 = [];
for (let i = 0; i < input.length; i++)
{
    let first = input[i][input[i].search(/[0-9]/)];
    //console.log(first)

    let last = -1;
    for (let j = input[i].length - 1; j >= 0; j--)
    {
        let temp = Number.parseInt(input[i][j]);
        if (!Number.isNaN(temp)) 
        {
            last = temp;
            break;
        }
    }

    (last == -1) && (last = first);

    output1[i] = first + last;
}


var sum = 0;
output1.forEach(x => sum += parseInt(x))
console.log(sum);

//Part 2
var output2 = [];
var numbers = {"1": "one", "2": "two", "3": "three", "4": "four", "5": "five", "6": "six", "7": "seven", "8": "eight", "9": "nine"}
for (let i = 0; i < input.length; i++)
{
    //Handle numbers
    let first = {"index": input[i].search(/[0-9]/), "value": input[i][input[i].search(/[0-9]/)]};

    let last = {"index": -1};
    for (let j = input[i].length - 1; j >= 0; j--)
    {
        let temp = Number.parseInt(input[i][j]);
        if (!Number.isNaN(temp)) 
        {
            last.index = j;
            last.value = temp;
            break;
        }
    }

    (last.index == -1) && (last = first);

    let keys = Object.keys(numbers);
    keys.forEach(key =>
        {
            value = numbers[key];

            if (input[i].indexOf(value) < first.index && input[i].indexOf(value) != -1) {
                first.index = input[i].indexOf(value);
                first.value = key
            }
            if (input[i].lastIndexOf(value) > last.index) {
                last.index = input[i].lastIndexOf(value);
                last.value = key;
            }
        })

    output2[i] = first.value + last.value;
}
var sum = 0;
output2.forEach(x => sum += parseInt(x))

console.log(sum);