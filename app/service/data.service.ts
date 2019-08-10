import { Injectable } from '@angular/core';
import { Select2OptionData } from 'ng2-select2'; 

@Injectable()
export class DataService {
    getChangeList(): Select2OptionData[] {
        return [
            {
                id: '0',
                text: 'Cars',
                children: [
                    {
                        id: 'car1',
                        text: 'Car 1'
                    },
                    {
                        id: 'car2',
                        text: 'Car 2'
                    },
                    {
                        id: 'car3',
                        text: 'Car 3'
                    }
                ]
            },
            {
                id: '0',
                text: 'Planes',
                children: [
                    {
                        id: 'plane1',
                        text: 'Plane 1'
                    },
                    {
                        id: 'plane2',
                        text: 'Plane 2'
                    },
                    {
                        id: 'plane3',
                        text: 'Plane 3'
                    }
                ]
            }
        ];
    }

    getChangeListAlternative(): Select2OptionData[] {
        return [
            {
                id: '0',
                text: 'Cars',
                children: [
                    {
                        id: 'car1',
                        text: 'Car 1 - New'
                    },
                    {
                        id: 'car2',
                        text: 'Car 2 - New'
                    },
                    {
                        id: 'car3',
                        text: 'Car 3 - New'
                    }
                ]
            },
            {
                id: '0',
                text: 'Planes',
                children: [
                    {
                        id: 'plane1',
                        text: 'Plane 1 - New'
                    },
                    {
                        id: 'plane2',
                        text: 'Plane 2 - New'
                    },
                    {
                        id: 'plane3',
                        text: 'Plane 3 - New'
                    }
                ]
            }
        ];
    }
}
