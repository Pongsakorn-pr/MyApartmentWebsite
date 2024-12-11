import React, { useState, useEffect } from "react";
import { Button, Table } from 'react-bootstrap';
import { PencilSquare, Trash } from 'react-bootstrap-icons';

import 'bootstrap/dist/css/bootstrap.min.css';

const DataPage = () => {
    const [data, setData] = useState([]);
    const [error, setError] = useState(null);

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
                setData(data); // Set the data in the state
            })
            .catch((error) => {
                setError(error.message); // Handle errors
            });
    }, []);

    return (
        <div className="container mt-4">
            <h1 className="mb-4">ข้อมูลหอพักทั้งหมด</h1>
            {error && <div className="alert alert-danger">{error}</div>} {/* Display error message */}

            <Table striped bordered hover responsive>
                <thead className="table-dark">
                    <tr>
                        <th>เลขที่บิล</th>
                        <th>เลขที่ห้อง</th>
                        <th>ประจำเดือน/ปี</th>
                        <th>ค่าห้อง</th>
                        <th>ค่ามิเตอร์เดือนก่อน</th>
                        <th>ค่ามิเตอร์เดือนปัจจุบัน</th>
                        <th>ส่วนต่างมิเตอร์</th>
                        <th>ค่าน้ำต่อหน่วย</th>
                        <th>ค่าขยะ</th>
                        <th>ค่าอื่นๆ</th>
                        <th>รวม</th>
                        <th>Actions</th> {/* Column for action buttons */}
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, rowIndex) => (
                        <tr key={rowIndex}>
                            <td>{item.bill_id}</td>
                            <td>{item.room_number}</td>
                            <td>{item.month}/{item.year}</td>
                            <td>{item.room_rent}</td>
                            <td>{item.previous_meter_month}</td>
                            <td>{item.water_reading_meter}</td>
                            <td>{item.water_diff}</td>
                            <td>{item.water_unit_fees}</td>
                            <td>{item.garbage_fees}</td>
                            <td>{item.other_fees}</td>
                            <td>{item.total_amount}</td>
                            <td>
                                <Button variant="warning" size="sm" className="mr-2">
                                    <PencilSquare />
                                </Button>
                                <Button variant="danger" size="sm">
                                    <Trash />
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};
const style = {
    // Table custom styles can go here if needed
    tableBill: {
        width: '100%',
        marginTop: '20px',
    },
};
export default DataPage;
