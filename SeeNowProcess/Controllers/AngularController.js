var taskApp = angular.module("taskForm", []);
taskApp.controller("taskCtrl", function ($scope) {
    $scope.tasks =
        [
            {
                title: "Aaa", description: "Bbb",
                status: "open", importance: "trivial",
                estimated_time: "2 weeks", parent: "s"
            }
        ];
    /* Roksana:
    1. Nie dodaję na razie "Choose an option" które dodała Ania, zostaje puste pole które po wyborze opcji
    znika z listy wyboru. Nie wiem na razie czy to dobry pomysł, zależy czy to będzie obowiązkowe pole (?)
    2. Chyba niepotrzebnie dodałam to jako obiekty, ale proszę o weryfikację */
    $scope.status_options =
        [
            { value: "open", description: "Open" },
            { value: "suspended", description: "Suspended" },
            { value: "closed", description: "Closed" }
        ];
    $scope.importance_options =
        [
             { value: "none", description: "None" },
             { value: "trivial", description: "Trivial" },
             { value: "regular", description: "Regular" },
             { value: "important", description: "Important" },
             { value: "critical", description: "Critical" }
        ];
    $scope.parent_options =
        [
             { value: "s", description: "Small (S)" },
             { value: "m", description: "Medium (M)" },
             { value: "l", description: "Large (L)" },
             { value: "xl", description: "Extra large (XL)" },
        ];
    $scope.addTask = function () {
        $scope.tasks.push({
            title: $scope.title, description: $scope.description, status: $scope.status.value,
            importance: $scope.importance.value, estimated_time: $scope.estimated_time, parent: $scope.parent.value
        });
    }
});