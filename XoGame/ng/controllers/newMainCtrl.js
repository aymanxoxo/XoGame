var app = angular.module('xoApp')
    .controller('newMainCtrl', function ($scope, $rootScope, $http) {
        $scope.player = {};
        $scope.game = {};
        $scope.tableData = {};
        $scope.tableData.tables = [];
        $scope.otherPlayerScore = 0;
        $scope.currentPlayerScore = 0;
        $scope.topTen = [];

        var myTurn = false;
        var symbol = "O";

        var connection = $.hubConnection();
        var hub = connection.createHubProxy('tableHub');

        hub.on('playerJoined', function (playerName) {
            if (playerName === $rootScope.currentPlayer.Name) return;
            $scope.otherPlayer = playerName;
            $scope.canStartMatch = true;

            $scope.selectedTable.PlayersCount += 1;

            if ($scope.selectedTable.PlayersCount === 2) $scope.matchStarted = true;

            $rootScope.$apply();
        });

        hub.on('updateTables', function () {
            $scope.getTables();
        });

        hub.on('tableAdded', function (table) {
            $scope.tableData.tables.push(table);
            $rootScope.$apply();
        });

        hub.on('played', function (location) {
            $scope.play(location, symbol === 'X' ? 'O' : 'X');
            myTurn = true;
            //$scope.checkIfWinner();
            $rootScope.$apply();
        });

        hub.on('playerLeft', function (playerName) {
            if ($scope.otherPlayer == playerName) {
                $scope.otherPlayer = '';
                $scope.canStartMatch = false;
                $scope.startMatch();
                $scope.getTables();
            }
        });

        hub.on('matchRestarted', function () {
            $scope.newGame();
            $rootScope.$apply();
        });

        hub.on('topTenChanged', function(topTen) {
            $scope.topTen = topTen;
            $rootScope.$apply();
        });

        connection.start();

        if (!$rootScope.currentPlayer) {
            $rootScope.isAuth = 0;

        }

        $scope.getTables = function () {
            $http.get('api/Table/GetAll')
                .success(function (data) {
                    if (data)
                        $scope.tableData.tables = data;
                });
        };
        $scope.getTables();

        $scope.getTopTen = function() {
            $http.get('api/Match/GetTopTen')
                .success(function(data) {
                    if (data)
                        $scope.topTen = data;
                });
        };
        $scope.getTopTen();


        $scope.logIn = function () {

            $http.post('api/Player/IsAuthorized', { Name: $scope.player.name, Password: $scope.player.password })
                .success(function (data) {
                    if (data) {
                        $rootScope.isAuth = 1;
                        $rootScope.currentPlayer = data;
                        hub.invoke('connect', data.Id, data.Name);
                    } else {
                        $scope.submitted = true;
                    }
                })
                .error(function (data) {
                    $scope.submitted = true;
                });
        };

        $scope.selectTable = function (table) {
            if (!table || ($scope.selectedTable && table.Name === $scope.selectedTable.Name)) return;

            $scope.selectedTable = table;

            if ($rootScope.currentPlayer && !$rootScope.currentPlayer.IsOwner) {
                $rootScope.currentPlayer.IsOwner = 0;
                if (table.Players && table.Players.length > 0) {
                    $scope.otherPlayer = table.Players[0].Name;
                    $scope.canStartMatch = true;
                    $rootScope.currentPlayer.IsOwner = 1;
                } else {
                    myTurn = true;
                    symbol = "X";
                }
            } else if ($rootScope.currentPlayer && $rootScope.currentPlayer.IsOwner) {
                myTurn = true;
            }

            $scope.title = $scope.selectedTable.Name;

            table.PlayersCount += 1;

            if (table.PlayersCount === 2) $scope.matchStarted = true;
            hub.invoke('joinTable', table.Name, $rootScope.currentPlayer.Id, $rootScope.currentPlayer.Name);

        }


        $scope.createTable = function () {
            //Create table physically on the database.
            $http.post('api/Table/AddTable', { Name: $scope.tableData.name })
                .success(function (data) {
                    if (!$scope.tableData.tables) $scope.tableData.tables = [];
                    $scope.tableData.tables.push(data);
                    $scope.tableData.saveError = null;
                    $rootScope.currentPlayer.IsOwner = 1;
                    symbol = "X";
                    myTurn = true;
                    $scope.saveError = null;
                    $scope.tableData.name = null;

                    //join the created table
                    data.PlayersCount = 0;
                    $scope.selectTable(data);

                    //publish the new created table to the other users
                    hub.invoke('TableCreatedEvent', JSON.stringify(data));
                })
                .error(function (error) {
                    $scope.saveError = error.ExceptionMessage;
                });
        }

        $scope.newGame = function () {
            $scope.game = {};
            $scope.winMessage = null;
            $scope.matchStarted = true;
            $scope.startMatchError = '';
        }

        $scope.startMatch = function () {
            if ($scope.canStartMatch) {
                $scope.newGame();
                hub.invoke('newMatch');
            }
            else
                $scope.startMatchError = "waiting for other player.";
        }


        $scope.play = function (obj, x) {
            var playSymbol = x ? x : symbol;
            if (!$scope.matchStarted || (!myTurn && !x)) return;

            switch (obj) {
                case 'aa':
                    {
                        if ($scope.game.aa) return;
                        $scope.game.aa = playSymbol;


                    } break;
                case 'ab':
                    {
                        if ($scope.game.ab) return;
                        $scope.game.ab = playSymbol;
                    } break;
                case 'ac':
                    {
                        if ($scope.game.ac) return;
                        $scope.game.ac = playSymbol;
                    } break;
                case 'ba':
                    {
                        if ($scope.game.ba) return;
                        $scope.game.ba = playSymbol;
                    } break;
                case 'bb':
                    {
                        if ($scope.game.bb) return;
                        $scope.game.bb = playSymbol;
                    } break;
                case 'bc':
                    {
                        if ($scope.game.bc) return;
                        $scope.game.bc = playSymbol;
                    } break;
                case 'ca':
                    {
                        if ($scope.game.ca) return;
                        $scope.game.ca = playSymbol;
                    } break;
                case 'cb':
                    {
                        if ($scope.game.cb) return;
                        $scope.game.cb = playSymbol;
                    } break;
                case 'cc':
                    {
                        if ($scope.game.cc) return;
                        $scope.game.cc = playSymbol;
                    } break;

                default:
            }
            if (!x)
                hub.invoke('play', obj, $scope.selectedTable.Name);

            myTurn = false;
            $scope.checkIfWinner();
        };

        $scope.checkIfWinner = function () {
            var winner = false;
            if ($scope.game.aa && $scope.game.ab && $scope.game.ac &&
                $scope.game.aa === $scope.game.ab && $scope.game.ab === $scope.game.ac)
                winner = $scope.game.aa;
            if ($scope.game.ba && $scope.game.bb && $scope.game.bc &&
                $scope.game.ba === $scope.game.bb && $scope.game.bb === $scope.game.bc)
                winner = $scope.game.ba;
            if ($scope.game.ca && $scope.game.cb && $scope.game.cc &&
                $scope.game.ca === $scope.game.cb && $scope.game.cb === $scope.game.cc)
                winner = $scope.game.ca;
            if ($scope.game.aa && $scope.game.bb && $scope.game.cc &&
                $scope.game.aa === $scope.game.bb && $scope.game.bb === $scope.game.cc)
                winner = $scope.game.aa;
            if ($scope.game.ac && $scope.game.bb && $scope.game.ca &&
                $scope.game.ac === $scope.game.bb && $scope.game.bb === $scope.game.ca)
                winner = $scope.game.ac;
            if ($scope.game.aa && $scope.game.ba && $scope.game.ca &&
                $scope.game.aa === $scope.game.ba && $scope.game.ba === $scope.game.ca)
                winner = $scope.game.aa;
            if ($scope.game.ab && $scope.game.bb && $scope.game.cb &&
                $scope.game.ab === $scope.game.bb && $scope.game.bb === $scope.game.cb)
                winner = $scope.game.ab;
            if ($scope.game.ac && $scope.game.bc && $scope.game.cc &&
                $scope.game.ac === $scope.game.bc && $scope.game.bc === $scope.game.cc)
                winner = $scope.game.ac;
            if (winner) {
                $scope.matchStarted = false;
                var whoWon = '';
                if (winner == symbol) {
                    whoWon = 'Congratulations, you';
                    $scope.currentPlayerScore++;
                    hub.invoke('win');
                } else {
                    whoWon = $scope.otherPlayer;
                    $scope.otherPlayerScore++;
                }
                $scope.winMessage = whoWon + " won!";
            }
        }

    });