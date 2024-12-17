import React, { useState, useEffect } from "react";
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

const DashboardPage = () => {
    const [sumvalue, setsum] = useState([]); // State to hold the data
    const [dashData, setDashData] = useState([]); // State for storing computed chart data
    const [error, setError] = useState(null); // For handling errors

    // Function to update values based on name
    const updateValue = (name, newValue) => {
        setDashData(prevData => {
            // Find the item by name and update its value
            const updatedData = prevData.map(item =>
                item.name === name ? { ...item, value: newValue } : item
            );
            return updatedData;
        });
    };

    useEffect(() => {
        // Fetch data from the backend API
        fetch("https://localhost:7054/api/Apartment") // Adjust API URL accordingly
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then((data) => {
                if (data.length > 0) {
                    setsum(data[0]); // Set the data in the state
                }
            })
            .catch((error) => {
                setError(error.message); // Handle errors
            });
    }, []); // This runs only once when the component mounts

    useEffect(() => {
        // Once sumvalue is updated, calculate the fees
        let sumWater_fees = 0;
        let sumRoom_rent = 0;
        let sumGarbage_fees = 0;
        let sumOther_fees = 0;

        // Assuming sumvalue is an array of objects
        sumvalue.forEach((item) => {
            sumWater_fees += item.water_diff * item.water_unit_fees;
            sumRoom_rent += item.room_rent;
            sumGarbage_fees += item.garbage_fees
            sumOther_fees += item.other_fees;

        });

        // Set up data for the Pie chart
        setDashData([
            { name: `Room_rent: ${sumRoom_rent}`, value: sumRoom_rent },
            { name: `Water: ${sumWater_fees}`, value: sumWater_fees },
            { name: `Garbage: ${sumGarbage_fees}`, value: sumGarbage_fees },
            { name: `Other: ${sumOther_fees}`, value: sumOther_fees },
        ]);

    }, [sumvalue]); // This runs whenever sumvalue changes

    return (
        <div style={styles.container}>
            <h2>Summary Chart</h2>
            <ResponsiveContainer width="100%" height={500}>
                <PieChart>
                    <Pie
                        data={dashData}
                        dataKey="value"
                        nameKey="name"
                        outerRadius={100}
                        fill="#8884d8"
                        label
                    >
                        {/* Map colors to the slices */}
                        {dashData.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />

                        ))} 
                    </Pie>
                    <Tooltip />
                    <Legend />
                </PieChart>
                
            </ResponsiveContainer>
        </div>
    );
};

const styles = {
    container: {
        padding: '20px',
        textAlign: 'center',
        height: '600px',
        width: '500px',
    },
};

export default DashboardPage;
